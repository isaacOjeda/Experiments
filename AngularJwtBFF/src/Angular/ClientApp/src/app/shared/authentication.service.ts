import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }


  saveUser(user: any) {
    localStorage.setItem('user', JSON.stringify(user));
  }

  getUser() {
    return JSON.parse(localStorage.getItem('user')!);
  }

  isAuthenticated() {
    return !!this.getUser();
  }

  login(username: string, password: string) {
    return this.http.post('local-login', { username, password });
  }

  logout() {
    this.http.post('local-logout', {}).subscribe(result => {
      localStorage.removeItem('user');
      window.location.href = '/login';
    });
  }
}
