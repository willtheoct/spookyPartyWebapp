import { Component } from '@angular/core';
import { timer } from 'rxjs/observable/timer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  passphrase = "";
  loginFailed = 0;

  tryLogin() {
    switch (this.passphrase) {
      case "urman": this.login("a"); break;
      case "yuan": this.login("b"); break;
      default: this.loginFailed++; timer(2000).subscribe(() => { this.loginFailed--; }); return;
    }
  }

  login(id = "") {
    localStorage.setItem("LoggedIn", "true");
    localStorage.setItem("loginId", id);
  }


  duel() {

  }
}
