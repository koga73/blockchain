//Finite Field
class FieldElement {
	constructor(num, prime) {
		if (num >= prime || num < 0) {
			throw new Error(`Number is not in range of 0 to ${prime}`);
		}
		this.num = num;
		this.prime = prime;
	}

	equals(fieldElement) {
		return fieldElement && fieldElement.num === this.num && fieldElement.prime === this.prime;
	}

	notEquals(fieldElement) {
		return !this.equals(fieldElement);
	}

	add(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		return new FieldElement((this.num + fieldElement.num) % this.prime, this.prime);
	}

	subtract(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		var num = (this.num - fieldElement.num) % this.prime;
		num = num < 0 ? num + this.prime : num;
		return new FieldElement(num, this.prime);
	}

	multiply(fieldElement) {
		const isFieldElement = fieldElement instanceof FieldElement;
		if (isFieldElement && fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		const num = isFieldElement ? fieldElement.num : fieldElement;
		return new FieldElement((this.num * num) % this.prime, this.prime);
	}

	pow(exponent = 2n) {
		const n = exponent % (this.prime - 1n);
		return new FieldElement(this.num ** n % this.prime, this.prime);
	}

	divide(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		return this.multiply(fieldElement.pow(fieldElement.prime - 2n));
	}

	clone(num = null, prime = null) {
		return new FieldElement(num !== null ? num : this.num, prime !== null ? prime : this.prime);
	}

	toString() {
		return `{num:${this.num}, prime:${this.prime}}`;
	}
}
export default FieldElement;
