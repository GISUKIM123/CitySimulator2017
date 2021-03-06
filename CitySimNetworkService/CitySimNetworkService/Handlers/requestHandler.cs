﻿using Newtonsoft.Json;
using NLog;
using System.Json;

namespace CitySimNetworkService
{
    class RequestHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DatabaseHandler dbHandler;
        private SimulationStateHandler simulationStateHandler;

        public RequestHandler(DatabaseHandler _dbHandler, SimulationStateHandler _simHandler)
        {
            dbHandler = _dbHandler;
            simulationStateHandler = _simHandler;
        }

        public JsonObject ParseRequest(string _request)
        {
            BaseRequest request = JsonConvert.DeserializeObject<BaseRequest>(_request, new RequestJsonConverter());
            switch (request.RequestType)
            {
                case "update":
                    return simulationStateHandler.HandleUpdateRequest((SimulationUpdateRequest) request);
                case "database":
                    return dbHandler.HandleRequest((DatabaseResourceRequest) request);
                default:
                    logger.Error("Invalid request made: {0}", _request);
                    //FIXME: This should return a JsonObject that contains request not valid field
                    return new JsonObject();
            }
        }
    }
}