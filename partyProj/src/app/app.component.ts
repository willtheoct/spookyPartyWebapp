import { Component, OnInit } from '@angular/core';
import { timer } from 'rxjs/observable/timer';
import { Router } from '@angular/router';
import { playerIds, PartyGoer } from '../models/partyGoer.model';
import { currency, currencies } from '../models/currency.model';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs/observable/interval';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class
  AppComponent implements OnInit {
  constructor(private http: HttpClient) {

  }

  mockplayers() {
    {
      let p = new PartyGoer();
      p.id = playerIds.adventurer;
      p.inventory[currencies.bronze] = 1;
      p.inventory[currencies.silver] = 1;
      p.inventory[currencies.starbucks] = 1;
      p.characterName = "Alex the Adventurer"
      PartyGoer.onlinePlayers.push(p);
    }
    {
      let p = new PartyGoer();
      p.id = playerIds.wizard;
      p.characterName = "will the wizard";
      p.inventory[currencies.rupees] = 1;
      p.inventory[currencies.ironOre] = 100;
      p.inventory[currencies.gold] = 3;
      PartyGoer.onlinePlayers.push(p);
    }
    {
      let p = new PartyGoer();
      p.id = playerIds.hargar;
      p.characterName = "hargar the harbinger"
      PartyGoer.onlinePlayers.push(p);
    }
    {
      let p = new PartyGoer();
      p.id = playerIds.asdf;
      p.characterName = "asdf"
      PartyGoer.onlinePlayers.push(p);
    }
    {
      let p = new PartyGoer();
      p.id = playerIds.overlord;
      p.characterName = "Ivan the Overlord"
      PartyGoer.onlinePlayers.push(p);
    }
  }

  ngOnInit(): void {
    this.http.get<PartyGoer[]>(AppComponent.hostServer + "players")
      .subscribe(x => PartyGoer.onlinePlayers = x);
    interval(10000).subscribe(() =>
      this.http.get<PartyGoer[]>(AppComponent.hostServer + "players")
        .subscribe(x => PartyGoer.onlinePlayers = x));
    AppComponent.userId = playerIds[localStorage.getItem("loginId")];
  }
  title = 'app';
  loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  passphrase = "";
  loginFailed = 0;
  static username = "";
  static userId: playerIds;
  static hostServer = "http://localhost:8220/"


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
    localStorage.setItem("loginId", playerIds[id]);
    this.loggedIn = true;
  }
}
