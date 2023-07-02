import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../shared/authentication.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  public currentUser: any;
  public products: any[] = [];

  constructor(
    private httpClient: HttpClient,
    private authService: AuthenticationService
  ) {

  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.getProducts();
    }

    this.currentUser = this.authService.getUser();
  }

  getProducts() {
    this.httpClient.get('/api/products').subscribe((response) => {
      this.products = response as any[];
    });
  }
}
