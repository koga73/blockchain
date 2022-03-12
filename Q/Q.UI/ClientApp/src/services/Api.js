const HOST = "https://localhost:7008";

class Api {
	static getUsers() {
		return fetch(`${HOST}/api/users`, {
			method: "GET"
		}).then((response) => response.json());
	}

	static registerUser(alias) {
		return fetch(`${HOST}/api/registeruser`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify({
				alias
			})
		}).then((response) => response.json());
	}

	static startMining(seed, alias) {
		return fetch(`${HOST}/api/startmining`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify({
				seed,
				alias
			})
		}).then((response) => response.json());
	}

	static isMining() {
		return fetch(`${HOST}/api/ismining`, {
			method: "GET"
		}).then((response) => response.json());
	}

	static stopMining() {
		return fetch(`${HOST}/api/stopmining`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			}
		}).then((response) => response.json());
	}

	static postMessage(alias, message) {
		return fetch(`${HOST}/api/postmessage`, {
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify({
				alias,
				message
			})
		}).then((response) => response.json());
	}

	static searchMessages(query) {
		let url = `${HOST}/api/searchmessages`;
		if (query) {
			url += `?q=${query}`;
		}
		return fetch(url, {
			method: "GET"
		}).then((response) => response.json());
	}
}
export default Api;
