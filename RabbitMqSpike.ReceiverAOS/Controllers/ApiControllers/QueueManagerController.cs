using System.Net;
using System.Net.Http;
using System.Web.Http;

using RabbitMqSpike.ReceiverAOS.Models;

namespace RabbitMqSpike.ReceiverAOS.Controllers.ApiControllers
{
    public class QueueManagerController : ApiController
    {
        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Get([FromUri] StatusModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                QueuedItems = 0,
                AutoStartServiceQueue = model.QueueName
            });
        }

        /// <summary>
        /// Start the Queue Processing
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody] StatusModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                QueuedItems = 0
            });
        }

        /// <summary>
        /// Stop
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Put([FromBody] StatusModel model)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                QueuedItems = 0
            });
        }
    }
}