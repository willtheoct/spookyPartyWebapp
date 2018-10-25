import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PartyGoer } from '../../models/partyGoer.model';

@Component({
  selector: 'app-contest',
  templateUrl: './contest.component.html',
  styleUrls: ['./contest.component.css']
})
export class ContestComponent implements OnInit {

  constructor(private http: HttpClient) { }

  players: PartyGoer[] = [];
  hasVoted = false;
  ngOnInit() {
    this.players = PartyGoer.onlinePlayers;
    if (localStorage.getItem("hasVotedForContest") === "true") {
      this.hasVoted = true;
    }
  }

  voteUp(p: PartyGoer) {
    var i = this.players.indexOf(p);
    if (i === this.players.length - 1) {
      return;
    }
    this.players[i] = this.players[i + 1];
    this.players[i + 1] = p;
  }
  voteDown(p: PartyGoer) {
    var i = this.players.indexOf(p);
    if (i === 0) {
      return;
    }
    this.players[i] = this.players[i - 1];
    this.players[i - 1] = p;
  }

}
