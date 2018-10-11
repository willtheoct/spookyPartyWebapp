import { Component } from '@angular/core';
import { timer } from 'rxjs/observable/timer';
import { Router } from '@angular/router';
import { playerIds } from '../models/partyGoer.model';

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
  static username = "";
  static userId;


  tryLogin() {
    switch (this.passphrase) {
      case "urman": this.login(playerIds.adventurer); break;
      case "yuan": this.login(playerIds.overlord); break;
      case "qq": this.login(playerIds.wizard); break;
      default: this.loginFailed++; timer(2000).subscribe(() => { this.loginFailed--; }); return;
    }
  }

  login(id: playerIds) {
    AppComponent.userId = id;
    AppComponent.username = playerIds[id];
    localStorage.setItem("LoggedIn", "true");
    localStorage.setItem("loginId", id.toString());
    this.loggedIn = true;
  }
}
