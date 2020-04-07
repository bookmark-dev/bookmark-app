import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { env } from 'process';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(private _http: HttpClient, private _url: string) {
    this._url = env['RestApiUrl'];
    if (this._url.length == 0) {
      this._url = 'https://localhost:5001';
    }
  }
  getOrganization(id: string) {
    return this._http.get(`${this._url}/api/org/${id}`);
  }
  getUser(id: string) {
    return this._http.get(`${this._url}/api/user/${id}`);
  }
}
