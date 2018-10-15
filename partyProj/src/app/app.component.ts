import { Component, OnInit } from '@angular/core';
import { timer } from 'rxjs/observable/timer';
import { Router } from '@angular/router';
import { playerIds, PartyGoer } from '../models/partyGoer.model';
import { currency, currencies } from '../models/currency.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class
  AppComponent implements OnInit {

  mockPlayerInventories() {
    {
      let p = new PartyGoer();
      let c = new currency();
      c.count = 1;
      c.type = currencies.gold;
      p.inventory.push(c)
      p.id = playerIds.adventurer;
      PartyGoer.onlinePlayers.push(p);
    }
    {
      let p = new PartyGoer();
      let c = new currency();
      c.count = 4;
      c.type = currencies.silver;
      p.inventory.push(c)
      p.id = playerIds.wizard;
      PartyGoer.onlinePlayers.push(p);
    }
    {
      let p = new PartyGoer();
      let c = new currency();
      c.count = 2;
      c.type = currencies.silver;
      p.inventory.push(c)
      p.id = playerIds.hargar;
      PartyGoer.onlinePlayers.push(p);
    }
  }

  ngOnInit(): void {
    this.mockPlayerInventories();
    AppComponent.userId = playerIds[localStorage.getItem("loginId")];
  }
  title = 'app';
  loggedIn = localStorage.getItem("LoggedIn") ? true : false;
  passphrase = "";
  loginFailed = 0;
  static username = "";
  static userId: playerIds;


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
