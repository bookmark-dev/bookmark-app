import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { env } from 'process';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  _url: string;
  constructor(private _http: HttpClient) {
    this._url = env['RestApiUrl'];
    if (this._url.length == 0) {
      this._url = 'https://localhost:5001';
    }
  }
  getUser(id: string) {
    return this._http.get(`${this._url}/api/user/${id}`);
  }
  getUserFromName(name: string) {
    return this._http.get(`${this._url}/api/user/name/${name}`);
  }
  postUser(name: string, password: string) {
    return this._http.post(`${this._url}/api/user`, 
      { Name: name, Password: password }
    );
  }
  comparePassword(password: string, hashed: string) {
    return this._http.post(`${this._url}/api/compare`,
      { password: password, hashed: hashed }
    );
  }
}
