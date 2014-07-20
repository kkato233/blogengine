using BlogEngine.Core.Packaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlogEngine.NET.AppCode.Api
{
    public class PackageExtraController : ApiController
    {
        public IEnumerable<PackageExtra> Get()
        {
            try
            {
                return Gallery.GetPackageExtras();
            }
            catch (UnauthorizedAccessException)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new HttpResponseException(HttpStatusCode.ExpectationFailed);

                var result = Gallery.GetPackageExtra(id);
                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (UnauthorizedAccessException)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public bool Rate(string id, [FromBody]Review review)
        {
            try
            {
                if (review == null) return false;
                if (review.Body.Length > 450) review.Body = review.Body.Substring(0, 450);

                return (BlogEngine.Core.Packaging.Gallery.RatePackage(id, review));
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
