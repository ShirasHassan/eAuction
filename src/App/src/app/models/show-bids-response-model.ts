

export interface ProductDetailsModel{
  id?: string;
  productName?: string;
  shortDescription?: string;
  detailedDescription?: string;
  category?: string;
  startingPrice?: string;
  bidEndDate?: Date;
  bids: ProductBidModel[];
}

export interface ProductBidModel
{
  id?: string;
  bidAmount?: string,
  buyerName?: string,
  email?: string,
  mobile?: string
}

export interface ShowBidsResponseModel {
  correlationId: string,
  auctionItem: ProductDetailsModel;
}
