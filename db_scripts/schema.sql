DROP TYPE clothing_items;
DROP TYPE clothing_types;

CREATE TYPE clothing_items AS ENUM (
    't_shirt', "long_sleeve" 'pants', 'socks', 'hat', 'underwear', 'watch', 'gloves', 'sweater', 'jacket', 'shorts'
);

CREATE TYPE clothing_types AS ENUM (
    'cotton', 'wool', 'leather', 'denim', 'silk', 'bamboo', 'polyester', 'nylon', 'spandex', 'rayon', 'acrylic'
);
