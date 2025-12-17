import { Event } from "../types";

export function parseLooseDate(dateStr: string): Date | null {
    if (!dateStr) return null;

    // YYYY-MM-DD
    let m = dateStr.match(/^(\d{4})-(\d{2})-(\d{2})$/);
    if (m) {
        const [, y, mo, d] = m;
        return new Date(Number(y), Number(mo) - 1, Number(d));
    }

    // ISO strings (T etc.)
    m = dateStr.match(
        /^(\d{4})-(\d{2})-(\d{2})(?:[T\s](\d{2}):(\d{2})(?::(\d{2}))?)?/i
    );
    if (m) {
        const [, y, mo, d, hh = "0", mm = "0", ss = "0"] = m;
        return new Date(Number(y), Number(mo) - 1, Number(d), Number(hh), Number(mm), Number(ss));
    }

    // fallback
    const nd = new Date(dateStr);
    return isNaN(nd.getTime()) ? null : nd;
}

export function eventEndDate(e: Event): Date | null {
    const base = parseLooseDate(e.date);
    if (!base) return null;
    const [hh = "23", mm = "59", ss = "59"] = (e.endTime || "").split(":");
    base.setHours(Number(hh), Number(mm), Number(ss), 0);
    return base;
}

export function isUpcoming(e: Event, now = new Date()): boolean {
    const end = eventEndDate(e);
    return end !== null && end >= now;
}
