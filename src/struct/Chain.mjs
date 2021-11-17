import * as crypto from "crypto";

import Block from "./Block.mjs";
import Transaction from "./Transaction.mjs";

class Chain {
	static instance = new Chain();

	constructor() {
		this.chain = [new Block({prevHash: null, transaction: new Transaction(100, "genesis", "satoshi")})];
	}

	getLastBlock() {
		return this.chain[this.chain.length - 1];
	}

	mine(nonce) {
		let solution = 1;
		console.log("Mining");

		while (true) {
			const hash = crypto.createHash("MD5");
			hash.update((nonce + solution).toString()).end();

			const attempt = hash.digest("hex");

			if (attempt.substr(0, 4) === "0000") {
				console.log(`SOLVED: ${solution}`);
				return solution;
			}

			solution += 1;
		}
	}

	addBlock({transaction, senderPublicKey, signature}) {
		const verifier = crypto.createVerify("SHA256");
		verifier.update(transaction.toString());

		const isValid = verifier.verify(senderPublicKey, signature);
		if (isValid) {
			const prevHash = this.getLastBlock().getHash();
			const newBlock = new Block({prevHash, transaction});
			this.mine(newBlock.nonce);
			this.chain.push(newBlock);
		}
	}
}
export default Chain;
