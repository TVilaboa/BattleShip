interface ID3Selectors {
    select (selector: string) : ID3Selection;
    select (node: EventTarget) : ID3Selection;
    selectAll: (selector: string) => ID3Selection;
}

interface ID3Base extends ID3Selectors {
    // Array Helpers
    ascending: (a: number, b: number) => number;
    descending: (a: number, b: number) => number;
    min<T, U>(arr: T[], map: (v: T) => U): U;
    min<T>(arr: T[]): T;
    max<T, U>(arr: T[], map: (v: T) => U): U;
    max<T>(arr: T[]): T;
    extent<T, U>(arr: T[], map: (v: T) => U): U[];
    extent<T>(arr: T[]): T[];
    quantile: (arr: number[], p: number) => number;
    bisectLeft<T>(arr: T[], x: T, low?: number, high?: number): number;
    bisect<T>(arr: T[], x: T, low?: number, high?: number): number;
    bisectRight<T>(arr: T[], x: T, low?: number, high?: number): number;

    // Loading resources
    xhr: {
        (url: string, callback: (xhr: XMLHttpRequest) => void): void;
        (url: string, mime: string, callback: (xhr: XMLHttpRequest) => void): void;
    };
    text: {
        (url: string, callback: (response: string) => void): void;
        (url: string, mime: string, callback: (response: string) => void): void;
    };
    json: (url: string, callback: (response: any) => void) => void;
    xml: {
        (url: string, callback: (response: Document) => void): void;
        (url: string, mime: string, callback: (response: Document) => void): void;
    };
    html: (url: string, callback: (response: DocumentFragment) => void) => void;
    csv: {
        (url: string, callback: (response: any[]) => void);
        parse(string: string): any[];
        parseRows(string: string, accessor: (row: any[], index: number) => any): any;
        format(rows: any[]): string;
    };

    time: ID3Time;
    scale: {
        linear(): ID3LinearScale;
        ordinal<Range>(): Ordinal<string, Range>;
        ordinal<Domain extends { toString(): string }, Range>(): Ordinal<Domain, Range>;
        category10(): Ordinal<string, string>;
        category10<Domain extends { toString(): string }>(): Ordinal<Domain, string>;
        category20(): Ordinal<string, string>;
        category20<Domain extends { toString(): string }>(): Ordinal<Domain, string>;
        category20b(): Ordinal<string, string>;
        category20b<Domain extends { toString(): string }>(): Ordinal<Domain, string>;
        category20c(): Ordinal<string, string>;
        category20c<Domain extends { toString(): string }>(): Ordinal<Domain, string>;

    };
    interpolate: ID3BaseInterpolate;
    interpolateNumber: ID3BaseInterpolate;
    interpolateRound: ID3BaseInterpolate;
    interpolateString: ID3BaseInterpolate;
    interpolateRgb: ID3BaseInterpolate;
    interpolateHsl: ID3BaseInterpolate;
    interpolateArray: ID3BaseInterpolate;
    interpolateObject: ID3BaseInterpolate;
    interpolateTransform: ID3BaseInterpolate;
    layout: ID3Layout;
    svg: ID3Svg;
    random: ID3Random;
    format(specifier: string): (n: number) => string;
}

interface ID3Selection extends ID3Selectors {
    attr: {
        (name: string): string;
        (name: string, value: any): ID3Selection;
        (name: string, valueFunction: (data: any, index: number) => any): ID3Selection;
    };

    classed: {
        (name: string): string;
        (name: string, value: any): ID3Selection;
        (name: string, valueFunction: (data: any, index: number) => any): ID3Selection;
    };

    style: {
        (name: string): string;
        (name: string, value: any, priority?: string): ID3Selection;
        (name: string, valueFunction: (data: any, index: number) => any, priority?: string): ID3Selection;
    };

    property: {
        (name: string): void;
        (name: string, value: any): ID3Selection;
        (name: string, valueFunction: (data: any, index: number) => any): ID3Selection;
    };

    text: {
        (): string;
        (value: any): ID3Selection;
        (valueFunction: (data: any, index: number) => any): ID3Selection;
    };

    html: {
        (): string;
        (value: any): ID3Selection;
        (valueFunction: (data: any, index: number) => any): ID3Selection;
    };

    append: (name: string) => ID3Selection;
    insert: (name: string, before: string) => ID3Selection;
    remove: () => ID3Selection;

    data: {
        (values: (data: any, index: number) => any): any;
        (values: any[], key?: (data: any, index: number) => any): ID3UpdateSelection;
    };

    call(callback: (selection: ID3Selection) => void): ID3Selection;
}

interface ID3EnterSelection {
    append: (name: string) => ID3Selection;
    insert: (name: string, before: string) => ID3Selection;
    select: (selector: string) => ID3Selection;
    empty: () => boolean;
    node: () => ID3Node;
}

interface ID3UpdateSelection extends ID3Selection {
    enter: () => ID3EnterSelection;
    update: () => ID3Selection;
    exit: () => ID3Selection;
}

interface ID3Time {
    second: ID3Interval;
    minute: ID3Interval;
    hour: ID3Interval;
    day: ID3Interval;
    week: ID3Interval;
    sunday: ID3Interval;
    monday: ID3Interval;
    tuesday: ID3Interval;
    wednesday: ID3Interval;
    thursday: ID3Interval;
    friday: ID3Interval;
    saturday: ID3Interval;
    month: ID3Interval;
    year: ID3Interval;

    seconds: ID3Range;
    minutes: ID3Range;
    hours: ID3Range;
    days: ID3Range;
    weeks: ID3Range;
    months: ID3Range;
    years: ID3Range;

    sundays: ID3Range;
    mondays: ID3Range;
    tuesdays: ID3Range;
    wednesdays: ID3Range;
    thursdays: ID3Range;
    fridays: ID3Range;
    saturdays: ID3Range;
    format: {

        (specifier: string): ID3TimeFormat;
        utc: (specifier: string) => ID3TimeFormat;
        iso: ID3TimeFormat;
    };

    scale(): ID3TimeScale;
}

interface ID3Range {
    (start: Date, end: Date, step?: number): Date[];
}

interface ID3Interval {
    (date: Date): Date;
    floor: (date: Date) => Date;
    round: (date: Date) => Date;
    ceil: (date: Date) => Date;
    range: ID3Range;
    offset: (date: Date, step: number) => Date;
    utc: ID3Interval;
}

interface ID3TimeFormat {
    (date: Date): string;
    parse: (string: string) => Date;
}

interface ID3LinearScale {
    (value: number): number;
    invert(value: number): number;
    domain(numbers: any[]): ID3LinearScale;
    range: {
        (values: any[]): ID3LinearScale;
        (): any[];
    };
    rangeRound: (values: any[]) => ID3LinearScale;
    interpolate: {
        (): ID3Interpolate;
        (factory: ID3Interpolate): ID3LinearScale;
    };
    clamp(clamp: boolean): ID3LinearScale;
    nice(): ID3LinearScale;
    ticks(count: number): any[];
    tickFormat(count: number): (n: number) => string;
    copy: ID3LinearScale;
}

interface ID3TimeScale {
    (value: Date): number;
    invert(value: number): Date;
    domain(numbers: any[]): ID3TimeScale;
    range: {
        (values: any[]): ID3TimeScale;
        (): any[];
    };
    rangeRound: (values: any[]) => ID3TimeScale;
    interpolate: {
        (): ID3Interpolate;
        (factory: ID3InterpolateFactory): ID3TimeScale;
    };
    clamp(clamp: boolean): ID3TimeScale;
    ticks: {
        (count: number): any[];
        (range: ID3Range, count: number): any[];
    };
    tickFormat(count: number): (n: number) => string;
    copy(): ID3TimeScale;
}

interface ID3InterpolateFactory {
    (a: any, b: any): ID3BaseInterpolate;
}
interface ID3BaseInterpolate {
    (a: any, b: any): ID3Interpolate;
}

interface ID3Interpolate {
    (t: number): number;
}

interface ID3Layout {


    stack(): ID3StackLayout;
    pack(): Pack<ID3Node>;
    pack<T extends ID3Node>(): Pack<T>;
}

interface ID3StackLayout {
    (layers: any[], index?: number): any[];
    values(accessor?: (d: any) => any): ID3StackLayout;
    offset(offset: string): ID3StackLayout;
}

interface ID3Svg {
    axis(): ID3SvgAxis;
}

interface ID3SvgAxis {
    (selection: ID3Selection): void;
    scale: {
        (): any;
        (scale: any): ID3SvgAxis;
    };

    orient: {
        (): string;
        (orientation: string): ID3SvgAxis;
    };

    ticks: {
        (count: number): ID3SvgAxis;
        (range: ID3Range, count?: number): ID3SvgAxis;
    };

    tickSubdivide(count: number): ID3SvgAxis;
    tickSize(major?: number, minor?: number, end?: number): ID3SvgAxis;
    tickFormat(formatter: (value: any) => string): ID3SvgAxis;
}

interface ID3Random {
    normal(mean?: number, deviation?: number): () => number;
}



interface Ordinal<Domain extends { toString(): string }, Range> {
    (x: Domain): Range;

    domain(): Domain[];
    domain(values: Domain[]): Ordinal<Domain, Range>;

    range(): Range[];
    range(values: Range[]): Ordinal<Domain, Range>;

    rangePoints(interval: [number, number], padding?: number): Ordinal<Domain, number>;
    rangeRoundPoints(interval: [number, number], padding?: number): Ordinal<Domain, number>;

    rangeBands(interval: [number, number], padding?: number, outerPadding?: number): Ordinal<Domain, number>;
    rangeRoundBands(interval: [number, number], padding?: number, outerPadding?: number): Ordinal<Domain, number>;

    rangeBand(): number;
    rangeExtent(): [number, number];

    copy(): Ordinal<Domain, Range>;
}


    interface ID3Node {
        parent?: ID3Node;
        children?: ID3Node[];
        value?: number;
        depth?: number;
        x?: number;
        y?: number;
        r?: number;
    }



    interface Link<T extends ID3Node> {
        source: ID3Node;
        target: ID3Node;
    }

interface Pack<T extends ID3Node> {
    (root: T): T[];

    nodes(root: T): T[];

    links(nodes: T[]): Link<T>[];

    children(): (node: T, depth: number) => T[];
    children(children: (node: T, depth: number) => T[]): Pack<T>;

    sort(): (a: T, b: T) => number;
    sort(comparator: (a: T, b: T) => number): Pack<T>;

    value(): (node: T) => number;
    value(value: (node: T) => number): Pack<T>;

    size(): [number, number];
    size(size: [number, number]): Pack<T>;

    radius(): number | ((node: T) => number);
    radius(radius: number): Pack<T>;
    radius(radius: (node: T) => number): Pack<T>;

    padding(): number;
    padding(padding: number): Pack<T>;
}

declare var d3: ID3Base;