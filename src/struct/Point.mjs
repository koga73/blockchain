import FieldElement from "./FieldElement.mjs";

//Point on an elliptic curve
//Works with real numbers and finite fields (FieldElement)
class Point {
	constructor(x, y, a, b) {
		this.x = x;
		this.y = y;
		this.a = a;
		this.b = b;
		this.isFieldElement = x instanceof FieldElement || y instanceof FieldElement || a instanceof FieldElement || b instanceof FieldElement;
		if (x === Infinity && y === Infinity) {
			return;
		}
		if (this.isFieldElement) {
			if (y.pow(2).notEquals(x.pow(3).add(a.multiply(x)).add(b))) {
				throw new Error("Point is not on the curve");
			}
		} else {
			if (Math.pow(y, 2) !== Math.pow(x, 3) + a * x + b) {
				throw new Error("Point is not on the curve");
			}
		}
	}

	equals(point) {
		if (this.isFieldElement) {
			return point && point.isFieldElement && point.x.equals(this.x) && point.y.equals(this.y) && point.a.equals(this.a) && point.b.equals(this.b);
		} else {
			return point && point.x === this.x && point.y === this.y && point.a === this.a && point.b === this.b;
		}
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

		if (this.isFieldElement) {
			//Is tangent?
			if (this.equals(point)) {
				//Tangent vertical line
				if (this.y.num === 0) {
					return new Point(Infinity, Infinity, this.a, this.b);
				} else {
					s = this.x.pow(2).multiply(3).add(this.a).divide(this.y.multiply(2));
					x = s.pow(2).subtract(this.x.multiply(2));
				}
			} else {
				s = point.y.subtract(this.y).divide(point.x.subtract(this.x));
				x = s.pow(2).subtract(this.x).subtract(point.x);
			}
			y = s.multiply(this.x.subtract(x)).subtract(this.y);
		} else {
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
		}

		return new Point(x, y, this.a, this.b);
	}

	toString() {
		return `{x:${this.x}, y:${this.y}, a:${this.a}, b:${this.b}}`;
	}
}
export default Point;
