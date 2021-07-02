class Point {
	constructor(x, y, a, b) {
		this.x = x;
		this.y = y;
		this.a = a;
		this.b = b;
		if (x === Infinity && y === Infinity) {
			return;
		}
		if (Math.pow(y, 2) !== Math.pow(x, 3) + a * x + b) {
			throw new Error("Point is not on the curve");
		}
	}

	equals(point) {
		return point && point.x === this.x && point.y === this.y && point.a === this.a && point.b === this.b;
	}

	add(point) {
		if (point.a !== this.a || point.b !== this.b) {
			throw new Error("Cannot manipulate points on different curves");
		}
		if (point.x === this.x && point.y !== this.y) {
			return new Point(Infinity, Infinity, this.a, this.b);
		}
		if (this.x === Infinity) {
			return new Point(point.x, point.y, point.a, point.b);
		}
		if (point.x === Infinity) {
			return new Point(this.x, this.y, this.a, this.b);
		}
		var s = 0;
		var x = Infinity;
		var y = Infinity;
		//Is tangent?
		if (this.equals(point)) {
			//Tangent vertical line
			if (this.y === 0) {
				return new Point(Infinity, Infinity, this.a, this.b);
			} else {
				s = (3 * Math.pow(this.x, 2) + this.a) / (2 * this.y);
				x = Math.pow(s, 2) - 2 * this.x;
			}
		} else {
			s = (point.y - this.y) / (point.x - this.x);
			x = Math.pow(s, 2) - this.x - point.x;
		}
		y = s * (this.x - x) - this.y;
		return new Point(x, y, this.a, this.b);
	}

	toString() {
		return `{x:${this.x}, y:${this.y}, a:${this.a}, b:${this.b}}`;
	}
}
export default Point;
