using System;
using System.Collections.Generic;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.DataAccess.Models;
using Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models;

namespace Rapidsoft.VTB24.Reports.Etl.DataAccess
{
    internal static class EtlDbContextExtensions
    {
        public static IEnumerable<EtlFile> GetRequestFilesItems(this EtlDbContext ctx, Guid packageId, DateTime fromDate, DateTime toDate)
        {
            return ctx.GetOutcomings()
                      .Where(outcoming => outcoming.Mail.EtlPackageId == packageId &&
                                          outcoming.Mail.Timestamp >= fromDate &&
                                          outcoming.Mail.Timestamp < toDate)
                      .AsEnumerable()
                      .Select(item => new EtlFile
                      {
                          Name = item.File.FileName,
                          Timestamp = item.Mail.Timestamp,
                          RowCount = item.RowCounter.CounterValue,
                          Size = item.SizeCounter != null ? item.SizeCounter.CounterValue : (long?) null
                      });
        }

        public static IEnumerable<Tuple<EtlFile, EtlFile>> GetRequestAndReplyFilesItems(this EtlDbContext ctx, Guid packageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix)
        {
            return
                ctx.GetIncomings()
                   .Where(incoming => incoming.Mail.EtlPackageId == packageId &&
                                      incoming.Mail.Timestamp >= fromDate &&
                                      incoming.Mail.Timestamp < toDate)
                   .JoinOutcomings(ctx.GetOutcomings(), replyFileNamePostfix)
                   .Select(item => new
                   {
                       inFileName = item.Incoming.File.FileName,
                       inTimestamp = item.Incoming.Mail.Timestamp,
                       inRowCount = (long?) item.Incoming.RowCounter.CounterValue,
                       hasOut = item.Outcoming.Mail != null,
                       outFileName = item.Outcoming.File.FileName,
                       outTimestamp = (DateTime?) item.Outcoming.Mail.Timestamp,
                       outRowCount = (long?) item.Outcoming.RowCounter.CounterValue,
                       outSizeCount = (long?) item.Outcoming.SizeCounter.CounterValue
                   })
                   .AsEnumerable()
                   .Select(item => Tuple.Create(
                       new EtlFile
                       {
                           Name = item.inFileName,
                           Timestamp = item.inTimestamp,
                           RowCount = item.inRowCount
                       },
                       item.hasOut
                           ? new EtlFile
                           {
                               Name = item.outFileName,
                               Timestamp = item.outTimestamp.Value,
                               RowCount = item.outRowCount,
                               Size = item.outSizeCount
                           }
                           : null));
        }

        public static IEnumerable<Tuple<EtlFile, EtlFile>> GetRequestAndReplyFilesItems(this EtlDbContext ctx, Guid requestPackageId, Guid repliesPackageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix)
        {
            return ctx.GetOutcomings()
                      .Where(outcoming => outcoming.Mail.EtlPackageId == requestPackageId &&
                                          outcoming.Mail.Timestamp >= fromDate &&
                                          outcoming.Mail.Timestamp < toDate)
                      .GroupJoin(ctx.GetIncomings()
                                    .Where(incoming => incoming.Mail.EtlPackageId == repliesPackageId),
                                 outcoming => (outcoming.File.FileName + replyFileNamePostfix).ToLower(),
                                 incoming => incoming.File.FileName.ToLower(),
                                 (outcoming, joined) => new
                                 {
                                     outcoming,
                                     reply = joined.OrderBy(incoming => incoming.Mail.Timestamp)
                                                   .FirstOrDefault(incoming => incoming.Mail.Timestamp >= outcoming.Mail.Timestamp)
                                 })
                      .Select(item => new
                      {
                          outFileName = item.outcoming.File.FileName,
                          outTimestamp = item.outcoming.Mail.Timestamp,
                          outRowCount = (long?) item.outcoming.RowCounter.CounterValue,
                          outSizeCount = (long?)item.outcoming.SizeCounter.CounterValue,
                          hasIn = item.reply.Mail != null,
                          inFileName = item.reply.File.FileName,
                          inTimestamp = (DateTime?) item.reply.Mail.Timestamp,
                          inRowCount = (long?) item.reply.RowCounter.CounterValue,
                      })
                      .AsEnumerable()
                      .Select(item => Tuple.Create(
                          new EtlFile
                          {
                              Name = item.outFileName,
                              Timestamp = item.outTimestamp,
                              RowCount = item.outRowCount,
                              Size = item.outSizeCount
                          },
                          item.hasIn
                              ? new EtlFile
                              {
                                  Name = item.inFileName,
                                  Timestamp = item.inTimestamp.Value,
                                  RowCount = item.inRowCount
                              }
                              : null));
        }

        public static IEnumerable<Tuple<EtlFile, EtlFile, EtlFile>> GetRequestAndTwoRepliesFilesItems(this EtlDbContext ctx, Guid requestPackageId, Guid repliesPackageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix1, string replyFileNamePostfix2)
        {
            var repliesItems = ctx.GetIncomings()
                                  .Where(incoming => incoming.Mail.EtlPackageId == repliesPackageId)
                                  .JoinOutcomings(ctx.GetOutcomings(), replyFileNamePostfix2);

            return ctx.GetOutcomings()
                      .Where(outcoming => outcoming.Mail.EtlPackageId == requestPackageId &&
                                          outcoming.Mail.Timestamp >= fromDate &&
                                          outcoming.Mail.Timestamp < toDate)
                      .GroupJoin(repliesItems,
                                 outcoming => (outcoming.File.FileName + replyFileNamePostfix1).ToLower(),
                                 repliesItem => repliesItem.Incoming.File.FileName.ToLower(),
                                 (outcoming, joined) => new
                                 {
                                     outcoming,
                                     replies = joined.OrderBy(repliesItem => repliesItem.Incoming.Mail.Timestamp)
                                                     .FirstOrDefault(repliesItem => repliesItem.Incoming.Mail.Timestamp >= outcoming.Mail.Timestamp)
                                 })
                      .Select(item => new
                      {
                          outFileName = item.outcoming.File.FileName,
                          outTimestamp = item.outcoming.Mail.Timestamp,
                          outRowCount = (long?) item.outcoming.RowCounter.CounterValue,
                          outSizeCount = (long?) item.outcoming.SizeCounter.CounterValue,
                          hasIn = item.replies.Incoming.Mail != null,
                          inFileName = item.replies.Incoming.File.FileName,
                          inTimestamp = (DateTime?) item.replies.Incoming.Mail.Timestamp,
                          inRowCount = (long?) item.replies.Incoming.RowCounter.CounterValue,
                          hasOut2 = item.replies.Outcoming.Mail != null,
                          out2FileName = item.replies.Outcoming.File.FileName,
                          out2Timestamp = (DateTime?) item.replies.Outcoming.Mail.Timestamp,
                          out2RowCount = (long?) item.replies.Outcoming.RowCounter.CounterValue,
                          out2SizeCount = (long?)item.replies.Outcoming.SizeCounter.CounterValue
                      })
                      .AsEnumerable()
                      .Select(item => Tuple.Create(
                          new EtlFile
                          {
                              Name = item.outFileName,
                              Timestamp = item.outTimestamp,
                              RowCount = item.outRowCount,
                              Size = item.outSizeCount
                          },
                          item.hasIn
                              ? new EtlFile
                              {
                                  Name = item.inFileName,
                                  Timestamp = item.inTimestamp.Value,
                                  RowCount = item.inRowCount
                              }
                              : null,
                          item.hasOut2
                              ? new EtlFile
                              {
                                  Name = item.out2FileName,
                                  Timestamp = item.out2Timestamp.Value,
                                  RowCount = item.out2RowCount,
                                  Size = item.out2SizeCount
                              }
                              : null));
        }

        private class IncomingMailWithFile
        {
            public EtlIncomingMail Mail { get; set; }

            public EtlIncomingFile File { get; set; }

            public EtlCounter RowCounter { get; set; }
        }

        private class OutcomingMailWithFile
        {
            public EtlOutcomingMail Mail { get; set; }

            public EtlOutcomingFile File { get; set; }

            public EtlCounter RowCounter { get; set; }

            public EtlCounter SizeCounter { get; set; }
        }

        private class IncomingWithOutcoming
        {
            public IncomingMailWithFile Incoming { get; set; }

            public OutcomingMailWithFile Outcoming { get; set; }
        }

        private static IQueryable<IncomingMailWithFile> JoinFiles(this IQueryable<EtlIncomingMail> mails, IEnumerable<EtlIncomingFile> files, IEnumerable<EtlCounter> counters)
        {
            return mails.GroupJoin(files,
                                   mail => new { mail.EtlPackageId, mail.EtlSessionId },
                                   file => new { file.EtlPackageId, file.EtlSessionId },
                                   (mail, mailFiles) => new { mail, mailFiles })
                        .SelectMany(item => item.mailFiles.Select(file => new { item.mail, file }))
                        .GroupJoin(counters,
                                   item => new { item.mail.EtlPackageId, item.mail.EtlSessionId, item.file.FileName },
                                   counter => new { counter.EtlPackageId, counter.EtlSessionId, FileName = counter.EntityName },
                                   (item, joined) => new IncomingMailWithFile
                                   {
                                       Mail = item.mail,
                                       File = item.file,
                                       RowCounter = joined.FirstOrDefault()
                                   });
        }

        private static IQueryable<OutcomingMailWithFile> JoinFiles(this IQueryable<EtlOutcomingMail> mails, IEnumerable<EtlOutcomingFile> files, IEnumerable<EtlCounter> rowCounters, IEnumerable<EtlCounter> sizeCounters)
        {
            return mails.GroupJoin(files,
                                   mail => new { mail.EtlPackageId, mail.EtlSessionId },
                                   file => new { file.EtlPackageId, file.EtlSessionId },
                                   (mail, mailFiles) => new { mail, mailFiles })
                        .SelectMany(item => item.mailFiles.Select(file => new { item.mail, file }))
                        .GroupJoin(rowCounters,
                                   item => new { item.mail.EtlPackageId, item.mail.EtlSessionId, item.file.FileName },
                                   counter =>
                                   new { counter.EtlPackageId, counter.EtlSessionId, FileName = counter.EntityName },
                                   (item, joined) => new { item.mail, item.file, rowCounter = joined.FirstOrDefault() })
                        .GroupJoin(sizeCounters,
                                   item => new { item.mail.EtlPackageId, item.mail.EtlSessionId, item.file.FileName },
                                   counter => new { counter.EtlPackageId, counter.EtlSessionId, FileName = counter.EntityName },
                                   (item, joined) => new { item.mail, item.file, item.rowCounter, sizeCounter = joined.FirstOrDefault() })
                        .Select(item => new OutcomingMailWithFile
                        {
                            Mail = item.mail,
                            File = item.file,
                            RowCounter = item.rowCounter,
                            SizeCounter = item.sizeCounter
                        });
        }

        private static IQueryable<IncomingMailWithFile> GetIncomings(this EtlDbContext ctx)
        {
            return ctx.IncomingMails.JoinFiles(ctx.IncomingFiles, ctx.Counters.Where(c => c.CounterName == "RowCount"));
        }

        private static IQueryable<OutcomingMailWithFile> GetOutcomings(this EtlDbContext ctx)
        {
            return ctx.OutcomingMails.JoinFiles(ctx.OutcomingFiles, ctx.Counters.Where(c => c.CounterName == "RowCount"), ctx.Counters.Where(c => c.CounterName == "CompressedSize"));
        }

        private static IQueryable<IncomingWithOutcoming> JoinOutcomings(this IQueryable<IncomingMailWithFile> incomings, IEnumerable<OutcomingMailWithFile> outcomings, string fileNamePostfix)
        {
            return incomings.GroupJoin(
                outcomings,
                incoming => new
                {
                    incoming.Mail.EtlSessionId,
                    FileName = (incoming.File.FileName + fileNamePostfix).ToLower()
                },
                outcoming => new
                {
                    outcoming.Mail.EtlSessionId,
                    FileName = outcoming.File.FileName.ToLower()
                },
                (incoming, joined) => new IncomingWithOutcoming
                {
                    Incoming = incoming,
                    Outcoming = joined.FirstOrDefault()
                });
        }
    }
}
