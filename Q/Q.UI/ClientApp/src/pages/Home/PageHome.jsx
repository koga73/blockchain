import React from "react";

import "./PageHome.scss";

function PageHome() {
	return (
		<section id="home">
			<h2>Messages</h2>
			<form id="frmSearch">
				<div className="text-wrap">
					<label htmlFor="txtSearch">Search:</label>
					<input id="txtSearch" name="txtSearch" type="search" placeholder="@handle, #hashtag, keyword" />
				</div>
				<button type="submit">
					<span className="hide-text">Search</span>
					<i className="fas fa-search"></i>
				</button>
			</form>
		</section>
	);
}
export default PageHome;
