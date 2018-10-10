import { Component } from '@angular/core';
import { timer } from 'rxjs/observable/timer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  loggedIn = false;
  passphrase = "";
  loginFailed = 0;

  login() {
    switch (this.passphrase) {
      case "urman": this.loggedIn = true; break;
      case "yuan": this.loggedIn = true; break;
      default: this.loginFailed++; timer(2000).subscribe(() => { this.loginFailed--; }); return;
    }
  }


  duel() {

  }
}
