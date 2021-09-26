using System;
using System.Threading.Tasks;
using eAuction.AuctionBC.Contract.Commands;
using eAuction.AuctionBC.Domain.AuctionItemAggregate;
using eAuction.BaseLibrary.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace eAuction.AuctionBC.EndPoint.Handlers
{
    /// <summary>
    /// DeleteAuctionItemCommandHandler
    /// </summary>
    public class DeleteAuctionItemCommandHandler : IConsumer<DeleteAuctionItemCommand>
    {

    readonly ILogger<DeleteAuctionItemCommandHandler> _logger;
    private readonly IAuctionRepository _auctionRepository;
    readonly IPublishEndpoint _endpoint;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="auctionRepository"></param>
    public DeleteAuctionItemCommandHandler(ILogger<DeleteAuctionItemCommandHandler> logger, IAuctionRepository auctionRepository, IPublishEndpoint endpoint)
    {
        _logger = logger;
        _auctionRepository = auctionRepository;
            _endpoint = endpoint;
        }

    /// <summary>
    /// Consumer/Handler
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Consume(ConsumeContext<DeleteAuctionItemCommand> context)
    {
            try
            {
                var auctionItemFilter = Builders<AuctionItem>.Filter.And(Builders<AuctionItem>.Filter.Eq(c => c.Id, context.Message.AuctionItemId),
                    Builders<AuctionItem>.Filter.Eq(c => c.Status.Name, EntityStatus.Active.Name));
                var auctionItem = _auctionRepository.Find(auctionItemFilter)?.Project(x => new { x.BidEndDate, x.SellerId })?.FirstOrDefault();
                if (auctionItem == null)
                {
                     await _endpoint.Publish(new DeleteAuctionItemFailedEvent(context.Message.CorrelationId, "Unable to find active auction item"));
                    return;
                }
                if (auctionItem?.BidEndDate < DateTime.Now)
                {
                    await _endpoint.Publish(new DeleteAuctionItemFailedEvent(context.Message.CorrelationId, "Unable to delete aution item after auction end date"));
                    return;
                }
                _logger.LogInformation("Value: {Value}", context.Message);

                var filter = Builders<AuctionItem>.Filter.And(
                    Builders<AuctionItem>.Filter.Eq(c => c.Id, context.Message.AuctionItemId),
                    Builders<AuctionItem>.Filter.Size(p => p.Bids, 0)
                );
                var update = Builders<AuctionItem>.Update
                    .Set(l => l.Status, EntityStatus.Deleted)
                    .Set(l => l.LastUpdatedTime, DateTime.Now);
                var result = await _auctionRepository.UpdateOneAsync(filter, update);
                await _auctionRepository.UnitOfWork.SaveChangesAsync();
                if (result.ModifiedCount == 1)  {
                    await _endpoint.Publish(new AuctionItemDeletedEvent(context.Message.CorrelationId, context.Message.AuctionItemId, auctionItem.SellerId));
                }
                else
                {
                    await _endpoint.Publish(new DeleteAuctionItemFailedEvent(context.Message.CorrelationId, "Unable to delete aution item that contains active bids"));
                }
            }
            catch (Exception e)
            {
                await _endpoint.Publish(new DeleteAuctionItemFailedEvent(context.Message.CorrelationId, e.Message));
            }

        }
    }
}