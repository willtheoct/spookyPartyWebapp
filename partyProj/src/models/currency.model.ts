export enum currencies {
    bronze,
    silver,
    gold,
    platinum,
    trinkets,
    crystals,
    starbucks,
    fudgeTokens,
    keys,
    ironOre,
    rupees
}

export class currency {
    constructor(
        public type: currencies,
        public count = 0) { }
}