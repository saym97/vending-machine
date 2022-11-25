<script>
    import{currentUser} from '$lib/stores';
    import Communicator from '$lib/VendingMachineAPI';
    async function Deposit(amount){
        let response = await Communicator.user.deposit(amount);
        if(response.status == 200){
            $currentUser.deposit += amount;
        }
    }
</script>

<div class="stats shadow">
	<div class="stat place-items-center">
		<div class="stat-value text-lg">{$currentUser?.name}</div>
		<div class="stat-desc text-lg badge">{"Buyer"}</div>
	</div>

	<div class="stat place-items-center">
		<div class="stat-title text-lg">Deposit</div>
		<div class="stat-value text-secondary">{$currentUser?.deposit}</div>
		<div class="stat-desc text-secondary text-lg">cents</div>
	</div>

	<div class="stat place-items-center">
		<div class="stat-title">Insert Coins</div>
		<div class="flex gap-2">
			<button on:click={()=>Deposit(5)} class="btn btn-active">5</button>
			<button on:click={()=>Deposit(10)} class="btn">10</button>
			<button on:click={()=>Deposit(20)} class="btn">20</button>
			<button on:click={()=>Deposit(50)} class="btn">50</button>
			<button on:click={()=>Deposit(100)} class="btn">100</button>
		</div>
	</div>
</div>

