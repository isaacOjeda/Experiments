import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  loginModel: {
    userName?: string,
    password?: string
  } = {};
  constructor(private httpClient: HttpClient) {

  }

  login() {
    this.httpClient.post('/api/login', this.loginModel).subscribe((response) => {
      console.log(response);
    });
  }

  getProducts() {
    this.httpClient.get('/api/products').subscribe((response) => {
      console.log(response);
    });
  }
}
