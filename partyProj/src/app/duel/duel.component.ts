import { Component, OnInit, Input, Output } from '@angular/core';
import { PartyGoer } from '../../models/partyGoer.model';
import { PlayerlistComponent } from '../playerlist/playerlist.component';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { currency, currencies } from '../../models/currency.model';
import { interval } from 'rxjs/observable/interval';
import { Router } from '@angular/router';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { duel } from '../../models/duel.model';

@Component({
  selector: 'app-duel',
  templateUrl: './duel.component.html',
  styleUrls: ['./duel.component.css']
})
export class DuelComponent implements OnInit {
  ngOnInit(): void {

    this.http.get<duel[]>("duels?playerId=" + AppComponent.userId).subscribe(x => this.availableDuels = x);

    this.availableDuelists = PartyGoer.onlinePlayers.filter(x => x.id !== AppComponent.userId);
    this.thisPlayer = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);

    let regenPlaceholderFunc = (v: number) => {
      let x = ["Smash Bros?", "Go Fish?", "Swords?", "Staring Contest?", "Rock Paper Scissors?", "Chess?", "Fisticuffs?", "Chicken?", "Yu-Gi-Oh!?"];
      this.randomPlaceholder = "";
      for (let i = 0; i < 1; i++) {
        let y = x.splice(Math.round(Math.random() * (x.length - 1)), 1);
        this.randomPlaceholder += y + " ";
      }
    }
    regenPlaceholderFunc(0);
    interval(6000).subscribe(regenPlaceholderFunc);
  }

  constructor(private http: HttpClient, private router: Router) { }
  target: PartyGoer = null;
  referee: PartyGoer = null;
  thisPlayer: PartyGoer = null;

  selectRef(r: PartyGoer) {
    this.http.put(AppComponent.hostServer + "duelChallenge", {
      src: AppComponent.userId,
      target: this.target.id,
      referee: r.id,
      srcWager: this.myInventoryWager,
      targetWager: this.targetInventoryWager,
      description: this.description
    })
      .subscribe(() => {
        this.http.get(AppComponent.hostServer + "duelChallenge?playerId=" + AppComponent.userId).subscribe(x => console.log(x));
        this.router.navigate(['/']);
      },
        e => this.router.navigate(['/']));
  }
  selectTarget(t: PartyGoer) {
    this.target = t;
    this.availableRefs = this.availableDuelists.filter(x => x.id !== t.id && x.id !== this.thisPlayer.id);
    console.log(this.availableRefs);
  }
  availableRefs: PartyGoer[];
  availableDuelists: PartyGoer[];
  description: string = '';
  targetInventoryWager: currency[] = Object.values(currencies).slice(Object.keys(currencies).length / 2).map(x => new currency(x, 0));;
  myInventoryWager: currency[] = Object.values(currencies).slice(Object.keys(currencies).length / 2).map(x => new currency(x, 0));;
  randomPlaceholder = "";
  wagered = false;

  availableDuels: duel[] = [];
  selectedDuel: duel;
  playersOf(d: duel) {
    return PartyGoer.onlinePlayers.filter(x => x.id == d.src || x.id == d.target);
  }
  decideDuel(winner: PartyGoer) {
    this.http.delete(AppComponent.hostServer + "duel?winner=" + winner.id);
  }
}
