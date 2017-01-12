namespace RapidSoft.VTB24.BankConnector.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.Logging;

    using RapidSoft.Loaylty.PromoAction.WsClients.TargetAudienceService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public static class SegmentExtensions
    {
        public const string VipSegmentId = "VIP";
        public const string StandartSegmentId = "Standart";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SegmentExtensions));

        public static string GetSegmentId(ClientSegment? clientSegment)
        {
            switch (clientSegment)
            {
                case ClientSegment.VIP:
                    return VipSegmentId;
                case ClientSegment.noVIP:
                    return StandartSegmentId;
                default:
                    {
                        var mess = string.Format("Сегмент клиента {0} не поддерживается", clientSegment);
                        throw new NotSupportedException(mess);
                    }
            }
        }

        public static ClientSegment? GetSegment(string segmentId, bool throwIfInvalid = false)
        {
            switch (segmentId)
            {
                case VipSegmentId:
                    return ClientSegment.VIP;
                case StandartSegmentId:
                    return ClientSegment.noVIP;
                default:
                    {
                        if (throwIfInvalid)
                        {
                            var mess = string.Format("Сегмент клиента {0} не поддерживается", segmentId);
                            throw new NotSupportedException(mess);
                        }

                        return null;
                    }
            }
        }

        public static IEnumerable<ClientSegment> GetSegments(this IEnumerable<TargetAudience> targetAudiences)
        {
            return targetAudiences.Select(ta => GetSegment(ta.Id)).Where(s => s.HasValue).Select(s => s.Value);
        }

        public static void CallAssignClientSegment(this ITargetAudienceService targetAudienceService, IEnumerable<Segment> segments, EtlLogger.EtlLogger etlLogger)
        {
            try
            {
                var segmentParameters = new AssignClientSegmentParameters
                {
                    Segments = segments.ToArray(),
                    UserId = ConfigHelper.VtbSystemUser
                };

                etlLogger.InfoFormat(
                    "Вызов сервиса TargetAudienceService при регистрации клиентов, записей для обработки: ({0})",
                    segmentParameters.Segments.Sum(x => x.ClientIds.Length));

                var result = targetAudienceService.AssignClientSegment(segmentParameters);

                if (result == null)
                {
                    throw new Exception("TargetAudienceService.AssignClientSegment result is null");
                }

                if (!result.Success)
                {
                    throw new Exception(string.Format("TargetAudienceService.AssignClientSegment result.Success is false {0}", result.ResultDescription));
                }
            }
            catch (Exception ex)
            {
                etlLogger.Error("Ошибка TargetAudienceService.AssignClientSegment", ex);
                throw;
            }
        }

        public static void CallAssignClientSegment(this ITargetAudienceService targetAudienceService, Segment segment)
        {
            try
            {
                var segmentParameters = new AssignClientSegmentParameters
                                            {
                                                Segments = new[] { segment },
                                                UserId = ConfigHelper.VtbSystemUser
                                            };

                var message =
                    string.Format(
                        "Вызов сервиса TargetAudienceService при регистрации клиентов, записей для обработки: ({0})",
                        segmentParameters.Segments.Sum(x => x.ClientIds.Length));
                Logger.Info(message);

                var result = targetAudienceService.AssignClientSegment(segmentParameters);

                if (result == null)
                {
                    throw new Exception("TargetAudienceService.AssignClientSegment result is null");
                }

                if (!result.Success)
                {
                    throw new Exception(string.Format("TargetAudienceService.AssignClientSegment result.Success is false {0}", result.ResultDescription));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка TargetAudienceService.AssignClientSegment: " + ex.Message);
                throw;
            }
        }
    }
}
