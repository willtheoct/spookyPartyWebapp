import { Component, OnInit } from '@angular/core';
import { timer } from 'rxjs/observable/timer';
import { Router } from '@angular/router';
import { playerIds, PartyGoer } from '../models/partyGoer.model';
import { currency, currencies } from '../models/currency.model';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs/observable/interval';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class
  AppComponent implements OnInit {
  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
    this.http.get<PartyGoer[]>(AppComponent.hostServer + "players")
      .subscribe(x => {
        PartyGoer.onlinePlayers = x;
        let ids = localStorage.getItem("loginId");
        let id = Number.parseInt(ids);
        if (!isNaN(id)) {
          this.login(id);
        }
      });
    interval(10000).subscribe(() =>
      this.http.get<PartyGoer[]>(AppComponent.hostServer + "players")
        .subscribe(x => PartyGoer.onlinePlayers = x));
  }
  loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  static loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  passphrase = "";
  loginFailed = 0;
  static username = "";
  static userId: playerIds = 0;
  static hostServer = "http://localhost:8220/";


  tryLogin() {
    let id = PartyGoer.onlinePlayers.find(x => x.passPhrase === this.passphrase).id;
    if (isNullOrUndefined(id)) {
      this.loginFailed++; timer(2000).subscribe(() => { this.loginFailed--; }); return;
    }
    else {
      this.http.get(AppComponent.hostServer + "login?userId=" + id).subscribe(() => this.login(id));
    }
  }

  login(id: number) {
    console.log("logging in with ID " + id);
    AppComponent.userId = id;
    AppComponent.username = playerIds[id];
    localStorage.setItem("LoggedIn", "true");
    localStorage.setItem("loginId", id.toString());
    this.loggedIn = true;
    AppComponent.loggedIn = true;
  }
}
