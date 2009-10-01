class CreateProducts < ActiveRecord::Migration
  def self.up
    create_table :products do |t|
      t.column :product_name, :string, :limit => 100, :null => false
      t.column :product_type_id, :integer, :null => false
      t.column :description, :string, :null => false
      t.column :price, :float, :null => false
      t.timestamps
    end
  end

  def self.down
    drop_table :products
  end
end
