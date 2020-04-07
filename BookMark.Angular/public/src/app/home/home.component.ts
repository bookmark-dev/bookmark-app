import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { HttpService } from '../http.service';

function AccountModel() {
  return {
    Name: '',
    Email: '',
    Password: ''
  };
}

interface ErrorMessage {
  message: string,
  name: string,
  kind: string,
  path: string,
  value: string,
  properties: any
};

function generateErrorMessages(errors): Array<string> {
  let result: Array<string> = [];
  return result;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  uvm: any;
  messages: Array<string>;
  flash: boolean;
  constructor(
    private _http: HttpService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {}
  ngOnInit(): void {
    this.uvm = AccountModel();
    this.messages = [];
    this.flash = false;
  }
  loginUser() {
    let user_task = this._http.getUserFromName(this.uvm.Name);
    user_task.subscribe(data => {
      if (data.hasOwnProperty('errors')) {
        this.uvm = AccountModel();
        this.messages = generateErrorMessages(data['errors']);
        this.flash = true;
      } else {
        let result_task = this._http.comparePassword(this.uvm.Password, data['Password']);
        result_task.subscribe(data => {
          if (data.hasOwnProperty('result')) {
            let result: boolean = data['result'];
            if (result == true) {
              this.uvm = AccountModel();
              this.messages = [];
              this.flash = false;
              this._router.navigate(['account', data['UserID']]);
            }
          }
        });
      }
    });
  }
  registerUser() {
    this._http.postUser(this.uvm['Name'], this.uvm['Password']).subscribe(data => {
      console.log(data);
      this._router.navigate(['home']);
    });
  }
}
