import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductBidComponent } from './product-bid.component';

describe('ProductBidComponent', () => {
  let component: ProductBidComponent;
  let fixture: ComponentFixture<ProductBidComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductBidComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductBidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
