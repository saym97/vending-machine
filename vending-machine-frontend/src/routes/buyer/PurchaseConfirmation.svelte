<script>
  export let productName;
  export let amount;
  export let totalCost;
  export let change;
  export let BuyMore;
</script>

<div class="card my-4 bg-base-100 shadow-xl">
	<div class="card-body">
		<h2 class="card-title">Purchase Successfull!</h2>
		<p>Product: {productName}</p>
		<p>Amount: {amount}</p>
		<p>Total cost: {totalCost}</p>
		<p>Change: {change}</p>
		<div class="card-actions justify-end">
			<button on:click={BuyMore} class="btn btn-primary">Buy More</button>
		</div>
	</div>
</div>

