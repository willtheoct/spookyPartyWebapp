export enum currencies {
    gold,
    crystals,
    ironOre,/*
    bronze,
    silver,
    platinum,
    trinkets,
    starbucks,
    fudgeTokens,
    keys,
    rupees*/
}

export class currency {
    constructor(
        public type: currencies,
        public count = 0) { }
}