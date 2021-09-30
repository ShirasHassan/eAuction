import {HttpClient} from "@angular/common/http"
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ShowBidsResponseModel } from "../models/show-bids-response-model";
import { ShowProductsResponseModel } from "../models/show-products-response-model";
import { SellerContext} from "../constants/seller-context";


@Injectable({
  providedIn: "root",
})
export class SellerService {

constructor(private http: HttpClient){}

getSellerProducts(sellerEmailId:string):Observable<ShowProductsResponseModel>{
  return this.http.get<ShowProductsResponseModel>(`${SellerContext.BaseUrl}/${encodeURIComponent(sellerEmailId)}/products`);
}

getProductBidsDetails(productId:string):Observable<ShowBidsResponseModel>{
  return this.http.get<ShowBidsResponseModel>(`${SellerContext.BaseUrl}/show-bids/${encodeURIComponent(productId)}`);
}
}
