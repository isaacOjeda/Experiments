import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  customers: ICustomerModel[] = [];

  totalItems = 0;
  pageSize = 10;
  currentPage = 1;

  constructor(private http: HttpClient,@Inject('BASE_URL') private baseUrl: string) {
    this.getCustomers(this.pageSize, this.currentPage);
  }

  private getCustomers(pageSize: number, currentPage: number) {

    const skip = pageSize * (currentPage - 1);

    this.http.get<IPagedResult<ICustomerModel>>(`${this.baseUrl}api/customers?pageSize=${pageSize}&skip=${skip}`)
      .subscribe(results => {
        this.customers = results.results;
        this.totalItems = results.totalItems;
      });
  }
}

interface ICustomerModel {
  name: string;
  email: string;
  customerId: number;
}

interface IPagedResult<T> {
  results: T[];
  totalItems: number;
}
