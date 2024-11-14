export interface CashHolding {
    currency: string;
    value: number;
}

export interface StockHolding {
    ticker: string;
    amount: number;
}

export interface Holding {
    holdingId: number;
    holdingGroupId: number;
    type: "cash" | "stock";
    cashHolding?: CashHolding;
    stockHolding?: StockHolding;
}
