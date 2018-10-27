import { Component, OnInit } from '@angular/core';
import { timer } from 'rxjs/observable/timer';
import { Router } from '@angular/router';
import { playerIds, PartyGoer } from '../models/partyGoer.model';
import { currency, currencies } from '../models/currency.model';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs/observable/interval';
import { isNullOrUndefined } from 'util';
import { Observable } from 'rxjs/Observable';

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
    let ids = localStorage.getItem("loginId");
    let id = Number.parseInt(ids);
    if (!isNaN(id)) {
      this.login(id);
    }
    this.http.get<PartyGoer[]>(AppComponent.hostServer + "players")
      .subscribe(x => {
        PartyGoer.allPlayers = x;
        AppComponent.username = PartyGoer.allPlayers[AppComponent.userId].characterName;
      });
    this.http.get<PartyGoer[]>(AppComponent.hostServer + "onlinePlayers")
      .subscribe(x => {
        PartyGoer.onlinePlayers = x;
      });
    interval(10000).subscribe(() =>
      this.http.get<PartyGoer[]>(AppComponent.hostServer + "onlinePlayers")
        .subscribe(x => PartyGoer.onlinePlayers = x));
  }
  loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  static loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  passphrase = "";
  loginFailed = 0;
  static username = "";
  static userId: number = 0;
  static hostServer = "http://magiccrystalball:8220/";
  //static hostServer = "http://localhost:8220/";
  static playerObservers = [];


  tryLogin() {
    console.log(PartyGoer.allPlayers.map(x => x.id));
    let loginTarget = PartyGoer.allPlayers.find(x => x.passPhrase === this.passphrase.toLowerCase());
    if (isNullOrUndefined(loginTarget)) {
      this.loginFailed++; timer(2000).subscribe(() => { this.loginFailed--; }); return;
    }
    else {
      this.http.get(AppComponent.hostServer + "login?userId=" + loginTarget.id).subscribe(() => this.login(loginTarget.id));
    }
  }

  login(id: number) {
    console.log("logging in with ID " + id);
    AppComponent.userId = id;
    localStorage.setItem("LoggedIn", "true");
    localStorage.setItem("loginId", id.toString());
    this.loggedIn = true;
    AppComponent.loggedIn = true;
  }
}
