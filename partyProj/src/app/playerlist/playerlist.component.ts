import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { PartyGoer } from '../../models/partyGoer.model';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs/observable/interval'
import { AppComponent } from '../app.component';


@Component({
  selector: 'app-playerlist',
  templateUrl: './playerlist.component.html',
  styleUrls: ['./playerlist.component.css']
})
export class PlayerlistComponent implements OnInit {
  @Output()
  selectedPlayer = new EventEmitter<PartyGoer>();
  static _onlinePlayers: PartyGoer[] = [];
  @Input()
  players: PartyGoer[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {

  }

}
