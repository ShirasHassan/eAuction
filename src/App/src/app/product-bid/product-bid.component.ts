import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { ProductDetailsModel } from '../models/show-bids-response-model';
import { ProductModel } from '../models/show-products-response-model';
import { SellerService } from '../services/seller-service';

export interface PeriodicElement {
  name: string;
  bidAmount: string;
  email: string;
  mobile: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {bidAmount: '100', name: 'Hydrogen', email: 'abc@gmail.com', mobile: '1234567891'},
  {bidAmount: '200', name: 'Hydrogen', email: 'abc@gmail.com', mobile: '1234567891'},
  {bidAmount: '300', name: 'Hydrogen', email: 'abc@gmail.com', mobile: '1234567891'},
  {bidAmount: '400', name: 'Hydrogen', email: 'abc@gmail.com', mobile: '1234567891'},
  {bidAmount: '500', name: 'Hydrogen', email: 'abc@gmail.com', mobile: '1234567891'},
];



@Component({
  selector: 'app-product-bid',
  templateUrl: './product-bid.component.html',
  styleUrls: ['./product-bid.component.css']
})
export class ProductBidComponent implements OnInit {

  products: ProductModel[];
  sellerProductId: string;
  selectedProduct: Subject<string>;
  productInfo: ProductDetailsModel;


  private sellerEmailId: string = 'dua@example.com';

  constructor(private sellerService: SellerService) {
    this.products=[];
    this.sellerProductId =  "";
    this.productInfo =  {bids:[]};
    this.selectedProduct = new Subject<string>();
  }

  ngOnInit(): void {
    this.sellerService.getSellerProducts(this.sellerEmailId).subscribe(
      data => {
        console.log(JSON.stringify(data));
        this.products = data.products;
      }
    );
    this.selectedProduct.subscribe( id => {
      this.sellerService.getProductBidsDetails(id).subscribe(data =>{
        console.log(data);
        console.log(JSON.stringify(data));
        this.productInfo = data.auctionItem;
        this.dataSource = this.productInfo?.bids;
      });
    });
  }



  selectChange() {
    this.selectedProduct.next(this.sellerProductId);
}
  displayedColumns: string[] = ['bidAmount', 'name', 'email', 'mobile'];
  dataSource:any;

}
