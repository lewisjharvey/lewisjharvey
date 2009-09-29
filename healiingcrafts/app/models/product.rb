class Product < ActiveRecord::Base
  belongs_to :product_type
  
  validates_uniqueness_of :product_name
  
  validates_presence_of :product_name
  validates_presence_of :product_type_id
  validates_presence_of :description
  validates_presence_of :price
  
  validates_length_of :product_name, :in => 5..100
  validates_numericality_of :price, :on => :create, :greater_than => 0.0
end
