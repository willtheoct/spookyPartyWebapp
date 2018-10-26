import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-achievements',
  templateUrl: './achievements.component.html',
  styleUrls: ['./achievements.component.css']
})
export class AchievementsComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<number[]>(AppComponent.hostServer + "achievements?user=" + AppComponent.userId).
      subscribe(unlockedAchievements => {
        this.achievements = unlockedAchievements;

      });
  }
  achievements = [];
  has(a: number) {
    return true;
    //return this.achievements.some(x => x === a);
  }

  sneakthief = 1;
  showoff = 2;
  monsterhunter = 3;
  breaksomething = 4;
  devourer = 5;
  galois = 6;
  duelist = 7;
  champion = 8;
  grinding = 9;
  adventurer = 10;
  partyoffive = 11;
  hairywizard = 12;
  aimbot = 13;

}