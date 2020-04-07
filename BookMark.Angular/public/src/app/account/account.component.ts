import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../http.service';

function AccountDisplayEmpty() {
  return {
    Name: '',
    Email: '',
    Password: ''
  };
}

function AccountDisplay(user_data: any) {
  let result = AccountDisplayEmpty();
  result['Name'] = user_data['name'];
  result['Email'] = user_data['email'];
  result['Password'] = user_data['password'];
  return result;
}

function generateErrorMessages(errors: any): Array<string> {
  let result: Array<string> = [];
  errors.array.forEach(e => {
    result.push(e);
  });
  return result;
}

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  acct_display: any;
  messages: Array<string>;
  flash: boolean;
  constructor(
    private _http: HttpService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {}
  ngOnInit(): void {
    this._route.params.subscribe(params => {
      const id = params['id'];
      this._http.getUser(id).subscribe(data => {
        if (data.hasOwnProperty('error')) {
          this.acct_display = AccountDisplayEmpty();
          this.messages = generateErrorMessages(data['error']);
          this.flash = true;
        } else {
          this.acct_display = AccountDisplay(data);
          this.messages = [];
          this.flash = false;
        }        
      });
    });
  }
}
