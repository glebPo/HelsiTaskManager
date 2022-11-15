using HelsiTaskManager.WebAPI.Attributes;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;
using static HelsiTaskManager.WebAPI.Attributes.HelsiRigthCheckAttribute;

namespace HelsiTaskManager.WebAPI.Extensions
{
    public class HelsiRightsMiddleware
    {
        private RequestDelegate _next;

        public IUnitOfWork UnitOfWork { get; set; }

        public List<string> FromUri => new List<string> {
            HttpMethods.Get,
            HttpMethods.Delete
        };

        public HelsiRightsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<HelsiRigthCheckAttribute>();
            //TODO: change to predicate
            if (context.Request.Headers.TryGetValue("X-User-Id", out var xUserId) 
                && attribute != null)
            {
                var parseResult = await TryGetIdAsync(context);
                await (parseResult.isParsed 
                    ? CheckOwnerOrLinkedUserAsync(parseResult.id.GetValueOrDefault(), xUserId, attribute.Type == HelsiRightType.Owner)
                    : _next(context));
            }

            await _next(context);
        }

        /// <summary>
        /// Check rights
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ForbiddenException"></exception>
        private async Task CheckOwnerOrLinkedUserAsync(ObjectId id, string userId, bool justOwner = false)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId) 
                || !await UnitOfWork.TaskList.AnyAsync(x
                => x.Id == id
                    && (x.OwnerId == userObjectId || (!justOwner && x.LinkedUsers.Contains(userObjectId)))))
            {
                throw new ForbiddenException("You must be owner or linked user");
            }
        }

        private async Task<(bool isParsed, ObjectId? id)> TryGetIdAsync(HttpContext context)
        {
            ObjectId? id;
            var verb = context.Request.Method;
            var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var body = string.IsNullOrEmpty(bodyAsText)
                ? null
                : JsonSerializer.Deserialize<BaseRequest>(bodyAsText);
            id = FromUri.Contains(verb) && ObjectId.TryParse(context?.Request?.Query?["id"], out var objectId)
                ? objectId
                : body?.Id; 
            return (id is not null && id != ObjectId.Empty, id);
        }
    }
}
