
export interface ShowProductsResponseModel {
  correlationId: string;
  products: ProductModel[];
}

export interface ProductModel {
  id?:string;
  productName?: string;
}

