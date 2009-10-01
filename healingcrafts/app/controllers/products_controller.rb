class ProductsController < ApplicationController
  def index
    if !params[:product_type_id]
      @products = Product.find(:all, :order => 'product_name')
    else
      @products = Product.find(:all, :conditions => ["product_type_id = :product_type_id", { :product_type_id => params[:product_type_id] }], :order => 'product_name')
    end
  end

  def summary
  end
  
  def new
    @product = Product.new
  end
  
  def edit
    @product = Product.find(params[:id])
  end
  
  def create
    @product = Product.new(params[:product])

      if @product.save
        flash[:notice] = 'Product was successfully created.'
        redirect_to(product_url)
      else
        render :action => "new"
      end
  end
  
  def update
    @product = Product.find(params[:id])

      if @product.update_attributes(params[:product])
        flash[:notice] = 'Product was successfully updated.'
        redirect_to(product_url)
      else
        render :action => "edit"
      end
  end
  
  def destroy
    @product = Product.find(params[:id])
    @product.destroy

    redirect_to(product_url)
  end
end
