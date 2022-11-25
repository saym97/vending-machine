<script>
  let amount = 1;
  export let productId;
  export let name;
  export let cost;
  export let amountAvailable;
  export let Buy;
</script>

<div class="card w-42 bg-base-100 shadow-xl">
	<div class="card-body items-center">
		<h2 class="card-title">{name}</h2>
		<h3 class="card-title">Cost: {cost}</h3>
		<p>Amount Available : {amountAvailable}</p>
		<div class="py-3 px-4 gap-2 items-center flex bg-base-300 shadow-lg rounded-md my-3">
			<button on:click={()=>(amount >= amountAvailable)? amount : amount++} class="btn btn-circle">+1</button>
			<div class="stat-value text-secondary text-2xl">{amount}</div>
			<button on:click={()=>(amount <= 1) ? amount : amount--} class="btn btn-circle">-1</button>
		</div>
		<div class="card-actions justify-end">
			<button on:click={()=>Buy(productId,amount)} class="btn btn-primary btn-wide">Buy</button>
		</div>
	</div>
</div>
