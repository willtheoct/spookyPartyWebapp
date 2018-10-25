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
      subscribe(unlockedAchievementIds => this.achievements = this.allAchievements.filter(a => unlockedAchievementIds.some(x => x === this.allAchievements.indexOf(a))));
  }
  achievements = [];
  allAchievements = [
    { name: "come on ", description: "this means you stepped it up or flew fast like a falcon" },
    { name: "", description: "" }
  ];
}