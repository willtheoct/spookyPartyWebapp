import { currencies, currency } from "./currency.model";
import { notification } from "../app/dashboard/dashboard.component";

export enum playerIds {
    wallace,
    hargar,
    playerName,
    whethers,
    weaver,
    asdf,
    zxcv,
    qwer
}
export class PartyGoer {
    characterName: string;
    level = 1;
    id: number;
    inventory: number[] = Object.values(currencies).slice(Object.entries(currencies).length / 2).map(x => 0);
    passPhrase = "";
    achievements: number[];

    static allPlayers: PartyGoer[] = [];
    static onlinePlayers: PartyGoer[] = [];
}