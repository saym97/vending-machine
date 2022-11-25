<script>
    import { createEventDispatcher } from 'svelte';
    import Communicator from '$lib/VendingMachineAPI';
    let name,cost;

	const dispatch = createEventDispatcher();
    let error=null;
    async function CreateProduct(){
        error=null
        let response = await Communicator.product.create({name,cost})
        if(response.status == 200){
            dispatch('create');
        }else{
            error = await response.text();
            setTimeout(()=>error=null,2000);
        }
    }
</script>

<div class="card w-1/2 mx-auto my-2 bg-base-100 shadow-xl">
    <div class="card-body">
      <div class="flex gap-2">
        <input bind:value={name} type="text" placeholder="name" class="input input-bordered input-primary w-full max-w-xs" />
        <input bind:value={cost} type="number" placeholder="cost" min="0" step="5" class="input input-bordered input-primary w-full max-w-xs" />
      </div>
        <button on:click={CreateProduct} class="btn">Create Product</button>
    </div>
</div>

{#if error}
<div class="toast">
  <div class="alert alert-info">
    <div>
      <span>{error}</span>
		</div>
	</div>
</div>
{/if}