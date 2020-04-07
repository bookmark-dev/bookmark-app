import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../http.service';

function AccountModel() {
  return {
    Name: '',
    Email: '',
    Password: ''
  };
}

function generateErrorMessages(errors: any): Array<string> {
  let result: Array<string> = [];
  errors.array.forEach(e => {
    result.push(e);
  });
  return result;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  uvm_login: any;
  uvm_register: any;
  messages: Array<string>;
  flash: boolean;
  constructor(
    private _http: HttpService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {}
  ngOnInit(): void {
    this.uvm_login = AccountModel();
    this.uvm_register = AccountModel();
    this.messages = [];
    this.flash = false;
  }
  loginUser() {
    this.messages = [];
    this._http.getUserFromName(this.uvm_login.Name).subscribe(user_data => {
      console.log(user_data);
      if (!user_data.hasOwnProperty('password')) {
        this.uvm_login = AccountModel();
        this.messages = generateErrorMessages(user_data);
      } else {
        let result_task = this._http.comparePassword(this.uvm_login.Password, user_data['password']);
        result_task.subscribe(data => {
          this.uvm_login = AccountModel();
          this.messages = [];
          if (data == true) {  
            this._router.navigate(['account', user_data['userID']]);
          } else {
            this._router.navigate(['home']);
          }
        });
      }
    }, (error) => console.log(error), () => console.log('Complete'));
    this.flash = this.messages.length > 0;
  }
  registerUser() {
    this.messages = [];
    console.log(this.uvm_register);
    if (this.uvm_register.Name.length == 0) {
      this.messages.push('Name shouldn\'t be empty!');
    } else if (this.uvm_register.Password.length == 0) {
      this.messages.push('Password shouldn\'t be empty!'); 
    } else {
      this._http.postUser(this.uvm_register.Name, this.uvm_register.Password).subscribe(data => {
        console.log(data);
        this._router.navigate(['home']);
      });
    }
    this.flash = this.messages.length > 0;
  }
}
